import { usePessoa } from "../../hooks/usePessoa";
import type { Pessoa } from "../../models/pessoa";
import type { PageModel } from "./PessoaShare";

interface Props {
	model?: PageModel;
	onHide: () => {};
}

export function PessoaForm(props: Props) {
	const { create, update } = usePessoa();
	const show = props.model?.show ?? "create";
	const pessoa = (show == "update" ? props.model?.item : {}) as Pessoa;
	const { id, nome, cpf, sexo, email, nacionalidade } = pessoa;

	const dataNascimento = Date.fromString(pessoa?.nascimento, "dd/MM/yyyy");
	const data = show == "update" ? dataNascimento.toString("yyyy-MM-dd") : "";

	// console.log("pessoa", show, pessoa);

	function onSubmit(e: React.FormEvent<HTMLFormElement>) {
		e.preventDefault();
		e.stopPropagation();

		const formData = new FormData(e.currentTarget);
		const pessoa = Object.fromEntries(formData.entries()) as any as Pessoa;
		const data = pessoa.nascimento as any;

		pessoa.id = (props.model?.show == "create" ? undefined : pessoa.id) as any;

		pessoa.nascimento =
			data instanceof Date
				? (data.toString("dd/MM/yyyy") as any)
				: Date.is(data, "yyyy-MM-dd")
					? Date.fromToString(data, "yyyy-MM-dd", "dd/MM/yyyy")
					: data;

		const action = props.model?.show == "update" ? update : create;

		// console.log(props.model?.show, pessoa);

		action(pessoa);

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
