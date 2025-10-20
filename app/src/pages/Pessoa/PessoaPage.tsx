import { useRef, useState } from "react";
import { Icon, useLoading, useToast } from "../../components";
import { usePessoa } from "../../hooks/usePessoa";
import { useProxy } from "../../hooks/useProxy";
import type { Pessoa } from "../../models/pessoa";
import { PessoaForm } from "./PessoaForm";
import { PageModel } from "./PessoaShare";
import { PessoaTable } from "./PessoaTable";
import "./index.css";

export function PessoaPage() {
	const setToast = useToast();
	const [_, setLoading] = useLoading();
	const [filtro, setFiltro] = useState("");
	const [isDisabled, setDisabled] = useState(true);
	const inputRef = useRef<HTMLInputElement>(null);
	const model = useProxy(true, { ...new PageModel() });
	const swr = usePessoa({ nome: filtro });

	// if (swr.await) setLoading(true);
	// if (swr.error) return <div>{swr.error.message}</div>;

	const onClose = () => (model.show = "");

	function onModal(p?: Pessoa) {
		model.show = p ? "update" : "create";
		model.item = p;
	}

	function onFiltrar() {
		const value = inputRef.current?.value;
		setFiltro(value || "");
		setDisabled(true);
	}

	function onInput() {
		const equals = inputRef.current?.value === filtro;
		if (!equals && isDisabled) setDisabled(false);
		console.log({ equals, isDisabled });
	}

	onFiltrar.bind(PessoaPage);

	swr.on("pending", () => setLoading(true));

	swr.on("success", () => {
		setLoading(false);
		setToast("success", "Opeação realizada com sucesso!");
	});

	swr.on("failure", (error) => {
		setToast("failure", error.message);
	});

	return (
		<>
			<h1> Pessoas </h1>
			<section id="pessoa">
				<aside id="panel">
					Busca por nome
					<input ref={inputRef} onInput={onInput} />
					<button disabled={isDisabled} onClick={() => onFiltrar()}>
						Filtrar
					</button>
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
