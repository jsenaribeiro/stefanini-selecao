export interface Result<T = any> {
	value: T;
	status: number;
	message: string | null;
}

export interface API<S, C, U, D> {
	search: S;
	create: C;
	update: U;
	delete: D;
}
