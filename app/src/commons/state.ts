// import type { API } from "./api"

// type Search<E> = <A extends object = any>(...args: A[]) => E[]
// type Change<A> = (args: A) => void
// type Create<E> = Change<E>
// type Update<E, I> = Change<Partial<E> & { id: I }>
// type Delete<I> = Change<I>

// export abstract class StateApi<E,I> {

//    abstract sync() // query

//    abstract save(entity: E, id: I)

//    abstract load(args: object)

//    abstract drop(id: I)

//    abstract setup(keys: string[]) {

//    }

//    abstract target: {
//       isLoading: boolean
//       isFailure: boolean
//       exception: Error|null
//    }
// }
