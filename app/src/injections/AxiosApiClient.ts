import axios, { type AxiosInstance } from "axios";
import { injectable } from "inversify";
import { RestApi } from "../commons/rest";

@injectable()
export class AxiosApiClient<E, I> extends RestApi<E, I> {
	private axios!: AxiosInstance;

	override build() {
		this.axios = axios.create({ baseURL: this.route });
		return this;
	}

	override async search(args: Object) {
		const query = args ? args.toQueryString() : "";
		console.log("query", query);
		const result = await this.axios.get(query).then((x) => x.data);
		return result;
	}

	override create = (entity: E) => this.axios.post(this.route, entity).then((x) => x.data);

	override update = (entity: E & { id: I }) =>
		this.axios.put(`${this.route}/${entity.id}`, entity).then((x) => x.data);

	override delete = (id: I) => {
		try {
			return this.route.endsWith((id?.toString() ?? "")?.toString())
				? this.axios.delete(this.route)
				: this.axios.delete(`${this.route}/${id}`).then((x) => x.data);
		} catch (ex) {
			console.log("errou aqui");
			throw ex;
		}
	};
}
