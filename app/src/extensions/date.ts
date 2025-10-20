declare global {
	interface DateConstructor {
		fromToString(data?: string, from?: string, to?: string): string;
		fromString(data?: string): Date;
		fromString(data?: string, format?: string): Date;
		is(data?: any, format?: string): boolean;
	}

	interface Date {
		toString(format?: string): string;
	}
}

Date.fromString = function (value?: string, format?: string): Date {
	format ||= "yyyy-MM-dd";

	const dateDefault = new Date(0, 0, 0);
	const formatParts = format?.split(/[^A-Za-z]/) ?? [];
	const valueParts = value?.split(/\D/) ?? [];

	if (!value || !format) return dateDefault;

	if (formatParts.length !== valueParts.length) return dateDefault;

	const map = { dd: 0, MM: 0, yyyy: 0 };

	for (let i = 0; i < formatParts.length; i++) {
		const token = formatParts[i];
		const num = parseInt(valueParts[i], 10);

		if (token === "dd") map.dd = num;
		else if (token === "MM") map.MM = num;
		else if (token === "yyyy") map.yyyy = num;
	}

	if (!map.dd || !map.MM || !map.yyyy) return dateDefault;

	const d = new Date(map.yyyy, map.MM - 1, map.dd);

	if (Number.isNaN(d.getTime())) return dateDefault;

	return d;
};

Date.prototype.toString = function (format?) {
	if (!format) return this.toLocaleString();

	const pad = (num, size = 2) => num.toString().padStart(size, "0");

	const map = {
		yyyy: this.getFullYear(),
		MM: pad(this.getMonth() + 1),
		dd: pad(this.getDate()),
		HH: pad(this.getHours()),
		mm: pad(this.getMinutes()),
		ss: pad(this.getSeconds()),
	};

	return format.replace(/yyyy|MM|dd|HH|mm|ss/g, (token) => map[token]);
};

Date.is = function (value, format) {
	if (typeof value !== "string" || typeof format !== "string") return false;

	const formatParts = format.split(/[^A-Za-z]/);
	const valueParts = value.split(/\D/);

	if (formatParts.length !== valueParts.length) return false;

	const map = {} as any;

	for (let i = 0; i < formatParts.length; i++) {
		map[formatParts[i]] = parseInt(valueParts[i], 10);
	}

	const year = map.yyyy ?? map.yy;
	const month = (map.MM ?? 0) - 1;
	const day = map.dd ?? 1;

	const date = new Date(year, month, day);

	return date.getFullYear() === year && date.getMonth() === month && date.getDate() === day;
};

Date.fromToString = function (value?, from?, to?) {
	if (!value || !from || !to) return value ?? "";

	return Date.fromString(value, from).toString(to);
};
