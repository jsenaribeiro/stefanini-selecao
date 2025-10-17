import { Container } from "inversify"
import { RestApi } from "../commons/rest"
import { StateApi } from "../commons/state"
import { AxiosApiClient } from "./AxiosApiClient"
import { ReactQueryHandler } from "./ReactQueryHandler"

const ioc = new Container()

ioc.bind(RestApi).to(AxiosApiClient)
ioc.bind(StateApi).to(ReactQueryHandler)

export { ioc }