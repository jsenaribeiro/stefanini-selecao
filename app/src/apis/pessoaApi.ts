import { RestApi } from "../commons/rest";
import { ioc } from "../injections";
import type { Pessoa } from "../models/pessoa";

const url = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

export const pessoaApi = ioc
	.get(RestApi<Pessoa>)
	.setup(`${url}/api/pessoas`)
	.build();
