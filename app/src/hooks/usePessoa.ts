import { ioc } from "../injections";
import { pessoaApi } from "../apis/pessoaApi";
import { SWR } from "../commons/swr";
import type { Pessoa } from "../models";
import type { Paged } from "../commons/types";

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
