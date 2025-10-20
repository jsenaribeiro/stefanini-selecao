import { injectable } from "inversify";
import type { RestApi } from "./rest";
import type { Paged, primitive } from "./types";

export type EventSWR = "success" | "failure" | "pending";

@injectable()
export abstract class SWR<E, I = primitive> {
	abstract error: Error | null | undefined;
	abstract value: Paged<E> | undefined;
	abstract await: boolean;

	protected api?: RestApi<E>;
	protected keys: string[] = [];
	protected query: object = {};

	protected onPending: () => void = () => {};

	protected onSuccess: (_: any) => void = (..._) => {};

	protected onFailure: (_: Error) => void = (..._) => {};

	public setup(keys: string[], api: RestApi<E>): this {
		this.keys = keys;
		this.api = api;
		return this;
	}

	abstract build(query?: object): this;

	/** delete */
	abstract drop(id: I): Promise<E> | undefined;

	/** create */
	abstract save(entity: E): Promise<E> | undefined;

	/** update */
	abstract save(entity: E, id: I): Promise<E> | undefined;

	/** search (refetch) */
	abstract load(query?: any): Promise<undefined> | undefined;

	public on(type: "pending", call: () => void);
	public on(type: "success", call: (value: any) => void);
	public on(type: "failure", call: (error: Error) => void);
	public on(type: EventSWR, call: Function) {
		if (type == "success") this.onSuccess = call as any;
		if (type == "failure") this.onFailure = call as any;
		if (type == "pending") this.onPending = call as any;
	}
}
