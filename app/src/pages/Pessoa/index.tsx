

import { StateApi } from "../../commons/state"
import { ioc } from "../../injections"


export function Pessoas() {
   const stateApi = ioc.get(StateApi)

   stateApi.target.exception
}