import type { Pessoa } from "../../models/pessoa";

export class PageModel {
	public show: "create" | "update" | "" = "";

	public item?: Pessoa = undefined;
}
