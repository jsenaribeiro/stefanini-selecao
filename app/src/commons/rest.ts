import type { Result, API } from "./api"

type GET<E> = () => Promise<Result<E[]>>
type POST<E> = (entity: Omit<E, "id">) => Promise<Result<E>>
type PUT<E,I> = (entity: Partial<E> & { id: I }) => Promise<Result<E>>
type DELETE<I> = (id: I) => Promise<Result>

export abstract class RestApi<E,I> implements API<GET<E>, POST<E>, PUT<E,I>, DELETE<I>> {
   abstract search(args?: object): Promise<Result<E[]>>
   abstract create(entity: Omit<E, "id">): Promise<Result<E>>
   abstract update(entity: Partial<E> & { id: I }): Promise<Result<E>>
   abstract delete(id: I): Promise<Result>
   
   public import(args?: object): Promise<void> {
      throw new Error('RestApi import is not implemented')
   }

   public export(args?: object): Promise<E[]> {      
      throw new Error('RestApi export is not implemented')
   }
}