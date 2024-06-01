export function convertMsToSeconds(value: number | undefined): string {
    if (!value)
        return "";

    const Number = Intl.NumberFormat('en-US', {
        minimumFractionDigits: 3
    });

    return Number.format(value / 1000);
}

export function convertBytesToMegabytes(value: number | undefined): string {
    if (!value)
        return "";

    const Number = Intl.NumberFormat('en-US', {
        minimumFractionDigits: 3
    });

    return Number.format(value / 1024 / 1024);
}