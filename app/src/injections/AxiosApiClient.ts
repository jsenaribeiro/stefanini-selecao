import axios, { type AxiosInstance, type AxiosResponse } from "axios";
import { injectable, unmanaged } from "inversify";
import type { Result } from "../commons/api";
import { RestApi } from "../commons/rest";

type IResponse = AxiosResponse<any, any, any>;

const getResult = (res: IResponse): Result => ({
	value: res.status < 300 ? res.data : null,
	status: res.status,
	message: res.status < 300 ? null : res.data?.message || res?.data,
});

@injectable()
export class AxiosApiClient<E, I> extends RestApi<E, I> {
	private readonly axios: AxiosInstance;

	constructor(baseURL: string) {
		super();
		this.axios = axios.create({ baseURL });
	}

	override search = (args?: any) =>
		this.axios.get(args.toString("query")).then(getResult);

	override create = (route: string, entity: E) =>
		this.axios.post(route, entity).then(getResult);

	override update = (route: string, entity: E & { id: I }) =>
		this.axios.put(`${route}/${entity.id}`, entity).then(getResult);

	override delete = (route: string, id: I) =>
		route.endsWith((id as any)?.toString())
			? this.axios.delete(route).then(getResult)
			: this.axios.delete(`${route}/${id}`).then(getResult);
}
