import type { MutationFunction, UseMutationResult, UseQueryResult } from "@tanstack/react-query";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { type EventSWR, SWR } from "../commons/swr";
import type { Paged, primitive } from "../commons/types";

type Fn<A, R> = MutationFunction<R, A>;

// biome-ignore format: não quebrar a linha
interface CRUD<T> {
    create: UseMutationResult<T, any, Omit<T, "id">, any>;
    update: UseMutationResult<T, any, Partial<T> & { id: primitive; }, any>;
    delete: UseMutationResult<T, any, primitive, any>;
}

export class ReactQuerySWR<T, I> extends SWR<T, I> {
	private mutationCUD?: CRUD<T>;
	private queryResult?: UseQueryResult<Paged<T>, Error>;

	private get queryfied() {
		const params = this.query ? JSON.stringify(this.query) : "";
		const queries = this.keys.concat(params);
		return queries;
	}

	override build(query) {
		if (query && !this.query) this.query = query;
		else if (query) Object.merge(this.query, query, true);
		if (!this.api) throw "ReactQuerySWR: restApi está nulo";

		this.queryResult = useQuery<Paged<T>>({
			queryKey: this.queryfied,
			queryFn: () => this.api!.search(this.query),
		});

		const callback = (type: EventSWR, valueOrError: any) =>
			type === "failure" && type !== undefined
				? this.onFailure(valueOrError)
				: this.onSuccess(valueOrError);

		this.mutationCUD = {
			create: mutateFactory(this.keys, this.api.create, callback),
			update: mutateFactory(this.keys, this.api.update, callback),
			delete: mutateFactory(this.keys, this.api.delete, callback),
		};

		// this.queryResult.refetch();

		return this;
	}

	override drop(id) {
		this.onPending();

		return this.mutationCUD?.delete?.mutateAsync(id);
	}

	override save(entity, id?) {
		this.onPending();

		return id && entity.id
			? this.mutationCUD?.update.mutateAsync(entity)
			: this.mutationCUD?.create.mutateAsync(entity);
	}

	override load(query?: any) {
		this.onPending();

		if (query) this.query = query;
		const queryClient = useQueryClient();
		return queryClient.fetchQuery<any>(this.queryfied as any);
	}

	public get await() {
		return (
			this.queryResult?.isLoading ||
			this.mutationCUD?.create.isPending ||
			this.mutationCUD?.update.isPending ||
			this.mutationCUD?.delete?.isPending ||
			false
		);
	}

	// biome-ignore format: getter curto
	public get value() { return this.queryResult?.data; }

	// biome-ignore format: getter curto
	public get error() { return this.queryResult?.error; }
}

function mutateFactory<A, R>(
	queryKey: string[],
	mutationFn: Fn<A, R>,
	call?: (type: EventSWR, data: any) => void,
) {
	const client = useQueryClient();
	const params = { queryKey };

	function onError(error) {
		call?.apply(null, ["failure", error]);
	}

	function onSuccess(data) {
		client.invalidateQueries(params);
		call?.apply(null, ["success", data]);
	}

	return useMutation({ mutationFn, onSuccess, onError });
}
