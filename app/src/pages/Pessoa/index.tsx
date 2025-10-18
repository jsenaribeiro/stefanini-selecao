import { PessoaForm } from "./form";
import { PessoaTable } from "./table";
import type { Pessoa } from "../../models/pessoa";
import { usePessoa } from "../../hooks/usePessoa";
import { useProxy } from "../../hooks/useProxy";
import { Icon } from "../../components";
import { PageModel } from "./types";
import "./index.css";

export default function PessoaPage() {
	const model = useProxy(true, { ...new PageModel() });
	const { isLoading, error } = usePessoa(model.item);

	if (isLoading) return <progress />;
	if (error) return <div>{error.message}</div>;

	const onModal = () => (model.form = true);
	const onClose = () => (model.form = false) || (model.item = {} as any);

	function onEditar(p: Pessoa) {
		model.form = true;
		model.item = p;
	}

	return (
		<>
			<h1> Pessoas </h1>
			<section id="pessoa">
				<aside id="panel">
					Filtrar <input />
					<button onClick={onModal}>
						<Icon name="add_circle" />
						Adicionar
					</button>
					<br />
				</aside>
				<section id="form-command">
					{model.form && <PessoaForm model={model} onHide={onClose} />}
					<PessoaTable onEdit={onEditar} />
				</section>
			</section>
		</>
	);
}
