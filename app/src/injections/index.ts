import { Container } from "inversify";
import { RestApi } from "../commons/rest";
import { StateApi } from "../commons/state";
import { AxiosApiClient } from "./AxiosApiClient";
import { ReactQueryHandler } from "./ReactQueryHandler";

const BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:3000";

const iocContainer = new Container();

iocContainer
	.bind(RestApi)
	.toDynamicValue(() => new AxiosApiClient(BASE_URL))
	.inSingletonScope();

export const ioc = iocContainer;
