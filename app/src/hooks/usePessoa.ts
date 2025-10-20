import { pessoaApi } from "../apis/pessoaApi";
import { SWR } from "../commons/swr";
import type { Paged } from "../commons/types";
import { ioc } from "../injections";
import type { Pessoa } from "../models";

export function usePessoa(query?: object): SWR<Pessoa> {
	return ioc
		.get(SWR<Pessoa>)
		.setup(["pessoas"], pessoaApi)
		.build(query);
}

export const PESSOAS_DEFAULT: Paged<Pessoa> = {
	sum: 0,
	pages: 0,
	records: [],
	size: 0,
	number: 0,
};
