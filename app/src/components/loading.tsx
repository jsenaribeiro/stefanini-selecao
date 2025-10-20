import { createContext, useContext, useState } from "react";

type LoadingContextType = [boolean, (value: boolean) => void];

const LoadingContext = createContext<LoadingContextType | undefined>(undefined);

const style = `
@keyframes spin {
	from { transform: rotate(0deg); }
	to { transform: rotate(360deg); }
}

#loading {
	inset: 0;
	display: flex;
	position: fixed;
	backdrop-filter: blur(8px);
	background-color: rgba(0, 0, 0, 0.3);
	justify-content: center;
	align-items: center;
	z-index: 9999;
}

#loading > section {
	color: white;
	display: flex;
	align-items: center;
	font-size: 1.5rem;
	font-weight: 500;
}

#loading > section > div {
	width: 32px;
	height: 32px;
	border: 4px solid rgba(255, 255, 255, 0.4);
	border-top-color: white;
	border-radius: 50%;
	margin-right: 12px;
	animation: spin 1s linear infinite;
}`;

const LoadingOverlay = () => (
	<main id="loading">
		<section>
			<div />
			Carregando...
		</section>
		<style>{style}</style>
	</main>
);

export const LoadingProvider = ({ children }: { children: any }) => {
	const [isLoading, setLoading] = useState(false);
	return (
		<LoadingContext.Provider value={[isLoading, setLoading]}>
			{children}
			{isLoading && <LoadingOverlay />}
		</LoadingContext.Provider>
	);
};

export const useLoading = () => {
	const excecao = "useLoading deve ser usado dentro de LoadingProvider";
	const context = useContext(LoadingContext);
	if (context) return context;
	throw new Error(excecao);
};
