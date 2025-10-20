import { Icon } from "../../components";
import type { Pessoa } from "../../models/pessoa";
import { usePessoa } from "../../hooks/usePessoa";
import type { Paged } from "../../commons/types";
import { useState } from "react";

interface Props {
	onEdit: (e: Pessoa) => void;
}

export function PessoaTable(props: Props) {
	const [pagina, setPagina] = useState(1);
	const swr = usePessoa({ page: { length: 3, number: pagina } });
	const pessoas = swr.value;

	if (swr.error) return <div>{swr.error.message}</div>;
	if (swr.await) return <div>carregando...</div>;

	const [sum, size] = [pessoas?.sum || 0, pessoas?.size || 1];

	const paginas = Array.range(1, (sum / size || 1) + 1)
		.map((n) => [pessoas, n])
		.map((x) => x as [Paged<Pessoa>, number]);

	function onPaginar(n: number) {
		// TODO: fazer paginação
		setPagina(n);
	}

	const convertDataNascimento = (p) =>
		Date.fromToString(p.nascimento, "yyyy-MM-dd", "dd/MM/yyyy");

	const PageNumber = ([p, n]: [Paged<Pessoa>, number]) => (
		<span key={n} className="p-1">
			{n != p.number && <a onClick={() => onPaginar(n)}>{n}</a>}
			{n == p.number && <span className="page-selected">{n}</span>}
		</span>
	);

	const PessoaBody = (pessoa: Pessoa, i: number) => (
		<tr key={i}>
			<td>{pessoa.id?.slice(0, 7)}</td>
			<td>{pessoa.nome}</td>
			<td>{pessoa.cpf}</td>
			<td>{pessoa.email}</td>
			<td>{convertDataNascimento(pessoa)}</td>
			<td>{pessoa.nacionalidade}</td>
			<td style={{ textAlign: "center" }}>
				<Icon
					tooltip="editar"
					name="edit_note"
					onClick={() => props.onEdit(pessoa)}
				/>
				<Icon
					tooltip="deletar"
					name="delete"
					onClick={() => swr.drop(pessoa.id)}
				/>
			</td>
		</tr>
	);

	return (
		<>
			<table id="pessoa-table">
				<thead>
					<tr>
						<th>Id</th>
						<th>Nome</th>
						<th>CPF</th>
						<th>Email</th>
						<th>Nascimento</th>
						<td>País</td>
						<th>Ação</th>
					</tr>
				</thead>
				<tbody>{pessoas?.records.map(PessoaBody)}</tbody>
			</table>
			<section className="grid grid-cols-[1fr_1fr]">
				<div>{paginas.map(PageNumber)}</div>
				<div style={{ justifySelf: "end" }}>
					{pessoas?.sum} total de registros
				</div>
			</section>
		</>
	);
}
