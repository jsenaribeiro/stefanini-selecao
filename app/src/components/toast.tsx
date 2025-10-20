import { createContext, useCallback, useContext, useEffect, useState } from "react";
import { Icon } from "./icon";

type ToastType = "success" | "warning" | "failure" | "";

const DELAY_DEFAULT = 5000;

interface Props {
	show: boolean;
	type: ToastType;
	duration: number;
	message: string;
	onClose: () => void;
}

interface ContextProps {
	setToast: (type: ToastType, text: string, time?: number) => void;
}

export const ToastContext = createContext<ContextProps | undefined>(undefined);

export function useToast() {
	const context = useContext(ToastContext);
	if (context) return context.setToast;
	throw new Error("useToast deve ser usado dentro de um ToastProvider");
}

export function Toast(props: Props) {
	function effect() {
		const timer = setTimeout(props.onClose, props.duration || DELAY_DEFAULT);
		return () => clearTimeout(timer);
	}

	useEffect(effect, []);

	const iconMap = {
		success: "check_circle",
		warning: "info",
		failure: "cancel",
	};

	const [fgColor, bgColor] =
		props.type === "success"
			? ["#20582C", "#DAEFDE"]
			: props.type === "warning"
				? ["#7E660F", "#FFF4D4"]
				: ["#600B19", "#FADEE0"];

	const toastStyle: any = {
		top: "20px",
		left: "50%",
		gap: "0 10px",
		display: "grid",
		marginLeft: "25px",
		color: fgColor,
		background: bgColor,
		fontWeight: "bolder",
		position: "absolute",
		borderRadius: "4px",
		padding: "10px 30px",
		fontFamily: "'Fira sans'",
		gridTemplateColumns: "auto 1fr",
		transform: "translate(-50%, 10px)",
		boxShadow: "0 0 10px rgba(0,0,0,0.3)",
		transition: "opacity 0.3s ease-in-out",
		opacity: props.show ? 1 : 0,
	};

	if (!props.show) return undefined;

	const labelStyle = {
		borderLeft: `solid 1px ${fgColor}`,
		paddingLeft: "15px",
		fontWeight: "400",
	};

	const iconStyle = {
		paddingLeft: "0",
		marginLeft: "-15px",
	};

	return (
		<div style={toastStyle}>
			<Icon size="25px" name={iconMap[props.type]} style={iconStyle} />
			<section style={labelStyle}>{props.message}</section>
		</div>
	);
}

export const ToastProvider = ({ children }: { children: React.ReactNode }) => {
	const [show, setShow] = useState(false);
	const [message, setMessage] = useState("");
	const [delay, setDelay] = useState(DELAY_DEFAULT);
	const [type, setType] = useState<ToastType>("success");

	function callback(toastType: ToastType = "success", text: string, time = DELAY_DEFAULT) {
		setShow(true);
		setDelay(time);
		setMessage(text);
		setType(toastType);
		setTimeout(() => setShow(false), delay || DELAY_DEFAULT);
	}

	const setToast = useCallback(callback, []);

	return (
		<ToastContext.Provider value={{ setToast }}>
			{children}
			{show && (
				<Toast show={show} type={type} duration={delay} message={message} onClose={() => setShow(false)} />
			)}
		</ToastContext.Provider>
	);
};
