import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";
import type { PageModel } from "./PessoaShare";

interface Props {
	model?: PageModel;
	onHide: () => {};
}

export function PessoaForm(props: Props) {
	const pessoarSWR = usePessoa();
	const show = props.model?.show ?? "create";
	const pessoa = (show === "update" ? props.model?.item : {}) as Pessoa;
	const { id, nome, cpf, sexo, email, nacionalidade } = pessoa;
	const dataNascimento = Date.fromString(pessoa?.nascimento, "dd/MM/yyyy");
	const data = show === "update" ? dataNascimento.toString("yyyy-MM-dd") : "";

	function onSubmit(e: React.FormEvent<HTMLFormElement>) {
		e.preventDefault();
		e.stopPropagation();

		const isoDateFormat = "yyyy-MM-dd";
		const brasilDateFormat = "yyyy-MM-dd";
		const isUpdate = props.model?.show === "update";
		const formData = new FormData(e.currentTarget);
		const pessoa = Object.fromEntries(formData.entries()) as any as Pessoa;
		const data = pessoa.nascimento as any;

		pessoa.id = isUpdate ? pessoa.id : undefined;

		pessoa.nascimento =
			data instanceof Date
				? (data.toString(isoDateFormat) as any)
				: Date.is(data, isoDateFormat)
					? Date.fromToString(data, brasilDateFormat, isoDateFormat)
					: data;

		isUpdate ? pessoarSWR.save(pessoa, pessoa.id) : pessoarSWR.save(pessoa);

		if (props.model) props.model.item = undefined;

		e.currentTarget.reset();
		props.onHide();
	}

	return (
		<dialog open={!!props.model?.show}>
			<h2>Cadastrar pessoa</h2>
			<form onSubmit={onSubmit}>
				<fieldset>
					<input hidden name="id" defaultValue={id || ""} />
					<section>
						<label htmlFor="nome">Nome</label>
						<input required name="nome" defaultValue={nome} />
					</section>
					<section>
						<label htmlFor="cpf">CPF</label>
						<input name="cpf" defaultValue={cpf} />
					</section>
					<section>
						<label htmlFor="sexo">Sexo</label>
						<select name="sexo" defaultValue={sexo}>
							<option value="" disabled></option>
							<option value="M">Masculino</option>
							<option value="F">Feminino</option>
						</select>
					</section>
					<section>
						<label htmlFor="email">Email</label>
						<input name="email" type="email" defaultValue={email} />
					</section>
					<section>
						<label htmlFor="nacimento">Nascimento</label>
						<input required name="nascimento" type="date" defaultValue={data} />
					</section>
					<section>
						<label htmlFor="nacionalidade">Nacionalidade</label>
						<input name="nacionalidade" defaultValue={nacionalidade} />
					</section>
				</fieldset>

				<section className="pessoa-botoes">
					<button type="reset" className="default" onClick={props.onHide}>
						Cancelar
					</button>
					<button type="submit" className="primary">
						Confirmar
					</button>
				</section>
			</form>
		</dialog>
	);
}
