import { PessoaForm } from "./PessoaForm";
import { PessoaTable } from "./PessoaTable";
import type { Pessoa } from "../../models/pessoa";
import { usePessoa } from "../../hooks/usePessoa";
import { useProxy } from "../../hooks/useProxy";
import { Icon, useLoading, useToast } from "../../components";
import { useRef, useState } from "react";
import { PageModel } from "./PessoaShare";
import "./index.css";

export function PessoaPage() {
	const setToast = useToast();
	const [_, setLoading] = useLoading();
	const [filtro, setFiltro] = useState("");
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
	}

	onFiltrar.bind(PessoaPage);

	swr.on("pending", () => setLoading(true));

	swr.on("success", function () {
		setLoading(false);
		setToast("success", "Opeação realizada com sucesso!");
	});

	swr.on("failure", function (error) {
		setToast("failure", error.message);
	});

	return (
		<>
			<h1> Pessoas </h1>
			<section id="pessoa">
				<aside id="panel">
					Busca por nome
					<input ref={inputRef} />
					<button onClick={() => onFiltrar()}>Filtrar</button>
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
