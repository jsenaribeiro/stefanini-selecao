import type { API } from "./api"

type Search<E> = <A extends object = any>(...args: A[]) => E[]
type Change<A> = (args: A) => void
type Create<E> = Change<E>
type Update<E, I> = Change<Partial<E> & { id: I }>
type Delete<I> = Change<I>

export abstract class StateApi<E,I> 
   implements API<Search<E>, Create<E>, Update<E, I>, Delete<I>> {
   
   abstract search: Search<E>
   abstract create: Create<E>
   abstract update: Update<E, I>
   abstract delete: Delete<I>   

   abstract target: {
      isLoading: boolean
      isFailure: boolean
      exception: Error|null
   }
}