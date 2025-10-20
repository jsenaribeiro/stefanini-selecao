import { Container } from "inversify";
import { RestApi } from "../commons/rest";
import { AxiosApiClient } from "./AxiosApiClient";
import { SWR } from "../commons/swr";
import { ReactQuerySWR } from "./ReactQuerySWR";

const iocContainer = new Container();

iocContainer.bind(RestApi).to(AxiosApiClient).inSingletonScope();

iocContainer.bind(SWR).to(ReactQuerySWR).inSingletonScope();

export const ioc = iocContainer;
