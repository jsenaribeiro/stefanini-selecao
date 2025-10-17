import { RestApi } from "../commons/rest"
import { type Result } from "../commons/api"
import { QueryClient, useMutation, useQuery, type UseMutationResult, type UseQueryResult } from "@tanstack/react-query";
import { StateApi } from "../commons/state";
import { injectable } from "inversify";


type EntityUpdate<E, I> = Partial<E> & { id: I }

type Search<E> = UseQueryResult<Result<E[]>, Error>
type Change<E, R = E> = UseMutationResult<Result<E>, Error, R>
type Create<E> = Change<E, Omit<E, "id">>
type Update<E, I> = Change<E, Partial<E> & { id: I }>
type Delete<I> = Change<I>

interface ReactQueryApi<E, I> {
   search: Search<E> | null
   create: Create<E>
   update: Update<E, I>
   delete: Delete<I>
}

@injectable()
export class ReactQueryHandler<E, I> extends StateApi<E, I> {
   private readonly rqa: ReactQueryApi<E, I>
   private readonly client: QueryClient
   private readonly onSuccess: () => { }

   constructor(private api: RestApi<E, I>, readonly queryKey: string[]) {
      super()

      this.client = new QueryClient()

      this.onSuccess = () => this.client.invalidateQueries({ queryKey })

      const create = useMutation<Result<E>, Error, Omit<E, 'id'>>({
         mutationFn: api.create, onSuccess: this.onSuccess
      })

      const update = useMutation<Result<E>, Error, Partial<E> & { id: I }>({
         mutationFn: entity => api.update(entity), onSuccess: this.onSuccess
      })

      const remove = useMutation<Result, Error, I>({ 
         mutationFn: api.delete, onSuccess: this.onSuccess 
      })

      this.rqa = { create, update, delete: remove, search: null }
   }

   override search = <A extends object = any>(args: A) => {
      const queryURL = args.toString('query')

      const result = useQuery({
         queryKey: [this.queryKey, queryURL], 
         queryFn: () => this.api.search(args)
      })

      this.rqa.search = result

      return result.data?.value || []
   }
   
   override create = (entity: E) => this.rqa.create.mutate(entity) ?? entity
   override update = (entity: EntityUpdate<E, I>) => this.rqa.update.mutate(entity)
   override delete = (id: I) => this.rqa.delete.mutate(id)

   override get target() {
      const isLoading = this.rqa.search?.isLoading
          || this.rqa.search?.isPending
          || this.rqa.create.isPending
          || this.rqa.update.isPending
          || this.rqa.delete.isPending
      
      const isFailure = this.rqa.search?.isError
          || this.rqa.create.isError
          || this.rqa.update.isError
          || this.rqa.delete.isError

      const exception = this.rqa.search?.error
          || this.rqa.create.error
          || this.rqa.update.error
          || this.rqa.delete.error

      return { isLoading, isFailure, exception }
   }
}