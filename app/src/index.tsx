import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter } from "react-router-dom";
import { ToastProvider } from "./components/toast";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./extensions";
import { LoadingProvider } from "./components/loading";

const queryClient = new QueryClient();

const rootHTML = document.getElementById("root");

ReactDOM.createRoot(rootHTML!).render(
	<React.StrictMode>
		<BrowserRouter>
			<QueryClientProvider client={queryClient}>
				<LoadingProvider>
					<ToastProvider>
						<App />
					</ToastProvider>
				</LoadingProvider>
			</QueryClientProvider>
		</BrowserRouter>
	</React.StrictMode>,
);
