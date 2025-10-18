import { ioc } from "../injections";
import { RestApi } from "../commons/rest";
import type { Pessoa } from "../models/pessoa";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

function useQueryFactory<T>(queryKey: string[], queryFn: () => Promise<T[]>) {
	return useQuery({ queryKey, queryFn });
}

function mutateFactory<A, T = A>(
	queryKey: string[],
	mutationFn: (args: A) => Promise<T>,
) {
	const queryClient = useQueryClient();
	const onSuccess = () => queryClient.invalidateQueries({ queryKey });
	return useMutation({ mutationFn, onSuccess });
}

export function usePessoa(query?: object) {
	const keys = ["pessoas"];
	const params = query ? JSON.stringify(query) : "";
	const restApi = ioc.get(RestApi<Pessoa, string>);

	const search = useQueryFactory<Pessoa>(keys.concat(params), () =>
		restApi.search("/pessoas", query).then((x) => x.value),
	);

	const create = mutateFactory<Omit<Pessoa, "id">, Pessoa>(keys, (data) =>
		restApi.create("/pessoas", data).then((x) => x.value),
	);

	const update = mutateFactory<Partial<Pessoa>>(keys, (data) =>
		restApi.update(`/pessoas`, data).then((x) => x.value),
	);

	const remove = mutateFactory<string | number, boolean>(keys, (id) =>
		restApi.delete("/pessoas", id?.toString()).then((x) => x.value),
	);

	return {
		...search,
		create: create.mutate,
		update: update.mutate,
		delete: remove.mutate,
	};
}
