declare global {
	interface ArrayConstructor {
		range(min: number, max: number): number[];
	}
}

Array.range = function (min, max) {
	if (min > max) return [];
	return Array.from({ length: max - min + 1 }, (_, i) => i + min);
};
