import { PessoaForm } from "./PessoaForm";
import { PessoaTable } from "./PessoaTable";
import type { Pessoa } from "../../models/pessoa";
import { usePessoa } from "../../hooks/usePessoa";
import { useProxy } from "../../hooks/useProxy";
import { Icon } from "../../components";
import { PageModel } from "./PessoaShare";
import type { ValueEvent } from "../../commons/types";
import { useState } from "react";
import "./index.css";

export function PessoaPage() {
	const [filtro, setFiltro] = useState("");
	const model = useProxy(true, { ...new PageModel() });
	const { isLoading, error } = usePessoa(model.query);

	if (isLoading) return <progress />;
	if (error) return <div>{error.message}</div>;

	const onClose = () => (model.show = "");

	function onModal(p?: Pessoa) {
		model.show = p ? "update" : "create";
		model.item = p;
	}

	function onFiltrar(e) {
		const event = e as ValueEvent;
		const value = event.target.value;
		setFiltro(value);
	}

	return (
		<>
			<h1> Pessoas </h1>
			<section id="pessoa">
				<aside id="panel">
					Busca por nome <input value={filtro} onInput={onFiltrar} />
					<button>Filtrar</button>
					<button onClick={() => onModal(undefined)}>
						<Icon name="add_circle" />
						Incluir
					</button>
				</aside>
				<section id="form-command">
					<PessoaForm model={model} onHide={onClose} />
					<PessoaTable onEdit={onModal} />
				</section>
			</section>
		</>
	);
}
