import "./icon.css";

interface Props {
	name: string;
	tooltip?: string;
	outlined?: boolean;
	onClick?: () => void;
}

export function Icon(props: Props) {
	const icon = props.name;
	const hover = props.onClick ? "hover" : "";
	const outlined = props.outlined ? "-outlined" : "";
	const css = `icon material-icons${outlined} ${hover}`;

	return (
		<span title={props.tooltip} onClick={props.onClick} className={css}>
			{icon}
		</span>
	);
}
