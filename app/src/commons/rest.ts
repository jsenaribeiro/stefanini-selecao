import type { API, Result } from "./api";

type GET<E> = (route: string, args: object) => Promise<Result<E[]>>;
type POST<E> = (route: string, entity: Omit<E, "id">) => Promise<Result<E>>;
type PUT<E, I> = (route: string, entity: Partial<E> & { id: I }) => Promise<Result<E>>;
type DELETE<I> = (route: string, id: I) => Promise<Result>;

export abstract class RestApi<E, I = any> implements API<GET<E>, POST<E>, PUT<E, I>, DELETE<I>> {
	abstract search(route: string, args?: object): Promise<Result<E[]>>;
	abstract create(route: string, entity: Omit<E, "id">): Promise<Result<E>>;
	abstract update(route: string, entity: Partial<E>): Promise<Result<E>>;
	abstract delete(route: string, id: I): Promise<Result>;

	public import(route: string, args?: object): Promise<void> {
		throw new Error("RestApi import is not implemented");
	}

	public export(route: string, args?: object): Promise<E[]> {
		throw new Error("RestApi export is not implemented");
	}
}
