import { AxiosApiClient } from "../injections/AxiosApiClient";

const url = import.meta.env.VITE_API_BASE_URL || "http://localhost:3000";

export const pessoaApi = new AxiosApiClient(`${url}/pessoas`);
