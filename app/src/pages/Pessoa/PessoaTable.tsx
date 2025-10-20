import { css } from "@emotion/react";
import { useState } from "react";
import type { Paged, ValueEvent } from "../../commons/types";
import { Icon } from "../../components";
import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";

interface Props {
	onEdit: (e: Pessoa) => void;
}

export function PessoaTable(props: Props) {
	const [pagina, setPagina] = useState(1);
	const [linhas, setLinhas] = useState(10);
	const swr = usePessoa({ page: { length: linhas, number: pagina } });
	const pessoas = swr.value;

	if (swr.error) return <div>{swr.error.message}</div>;
	if (swr.await) return <div>carregando...</div>;

	const [sum, size] = [pessoas?.sum || 0, pessoas?.size || 1];

	const paginas = Array.range(1, (sum / size || 1) + 1)
		.map((n) => [pessoas, n])
		.map((x) => x as [Paged<Pessoa>, number]);

	const convertDataNascimento = (p) =>
		Date.fromToString(p.nascimento, "yyyy-MM-dd", "dd/MM/yyyy");

	function onSetPageSize(e) {
		const value = parseInt(e.target.value) || 10;
		setLinhas(value);
	}

	const PageNumber = ([p, n]: [Paged<Pessoa>, number]) => (
		<span key={n}>
			{n != p.number && <a onClick={() => setPagina(n)}>{n}</a>}
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
			<section css={pageStyle} cols="auto 1fr 1fr">
				<div cols="auto auto">
					<label></label>
					<select onChange={onSetPageSize} value={linhas}>
						<option value="2">2</option>
						<option value="10">10</option>
						<option value="25">25</option>
						<option value="50">50</option>
						<option value="75">75</option>
						<option value="100">100</option>
					</select>
				</div>
				<div className="page">{paginas.map(PageNumber)}</div>
				<div className="page counter">{pessoas?.sum} total de registros</div>
			</section>
		</>
	);
}

const pageStyle = css`	
	div.page.counter {
		text-align: right;
		justify-self: end;
	}

	div.page {
		margin-left: 15px;
	}

	div.page > span {
		font-weight: bolder;
		margin-top: 5px;
	}

	div.page > span > a,
	div.page > span > a:hover,
	div.page > span > span {
		display: inline-block;
		padding: 0px 10px !important;
		border: solid 1px #444;
		line-height: 35px;
	}

	div.page > span > span {
		color: var(--inverse);
		background: var(--primary);
		font-weight: bolder;
	}
`;
