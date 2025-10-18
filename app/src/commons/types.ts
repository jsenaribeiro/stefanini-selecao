import type { Result } from "./api";

export type ChangeType = "create" | "update" | "delete";

export type Async = (route: string, args: any) => Promise<Result<any>>;

export interface ValueEvent {
	target: {
		value: string;
	};
}
