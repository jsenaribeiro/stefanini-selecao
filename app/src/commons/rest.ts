import type { Paged, primitive } from "./types";

export abstract class RestApi<E, I = primitive> {
	protected route: string = "";

	public setup(route: string): this {
		if (route.endsWith("/")) route = route.slice(0, -1);
		this.route = route;
		return this;
	}

	abstract build(): this;

	abstract search(args?: object): Promise<Paged<E>>;
	abstract create(entity: Omit<E, "id">): Promise<E>;
	abstract update(entity: Partial<E> & { id: I }): Promise<E>;
	abstract delete(id: I): Promise<E>;

	// public import(route?: string, args?: object): Promise<void> {
	// 	throw new Error("RestApi import is not implemented");
	// }

	// public export(route?: string, args?: object): Promise<E[]> {
	// 	throw new Error("RestApi export is not implemented");
	// }
}
