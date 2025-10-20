export type primitive =
	| string
	| number
	| boolean
	| bigint
	| symbol
	| null
	| undefined;

export type record<T = primitive> = Record<string, T>;

export type ChangeType = "create" | "update" | "delete";

export type Async = (route: string, args: object) => Promise<Result<object>>;

export type Mapper<E, K extends keyof E = keyof E> = (entity: E) => E[K];

export type ValueEvent = { target: { value: string } };

export interface Result<T = object> {
	ok: boolean;
	size: number;
	value: T;
	status: number;
	message: string | null;
}

export interface Paged<E> {
	sum: number; // soma do total de paginas da consulta
	size: number; // quantidade de linhas por pagina
	pages: number; // quantidade de páginas
	number: number; // número da página
	records: E[]; // conteúdo da consulta
}
