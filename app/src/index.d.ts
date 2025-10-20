import type { SerializedStyles } from "@emotion/react";

declare global {}
declare module "@emotion/react" {
	export interface Theme {}
}

declare module "react" {
	interface Attributes {
		css?: SerializedStyles;
		cols?: string;
	}
}
