import axios, { type AxiosInstance, type AxiosResponse } from "axios";
import { API, type Result } from "../commons/api";
import { RestApi } from "../commons/rest";
import { injectable } from "inversify";

type IResponse = AxiosResponse<any, any, any>

const getResult = (res: IResponse): Result => ({ 
   value: res.status < 300 ? res.data : null,
   status: res.status,  
   message: res.status < 300 ? null : (res.data?.message || res?.data),
})

@injectable()
export class AxiosApiClient<E,I> extends RestApi<E,I> {
   private readonly axios: AxiosInstance

   constructor(baseURL: string) { 
      super()
      this.axios = axios.create({ baseURL }) 
   }
   
   override search = (args?: any) => this.axios.get(args.toString('query')).then(getResult)
   
   override create = (entity: E) => this.axios.post('', entity).then(getResult)   
   
   override update = (entity: E & { id: I; }) =>  this.axios.put(`/${entity.id}`, entity).then(getResult)

   override delete = (id: I) => this.axios.delete(`/${id}`).then(getResult)
}