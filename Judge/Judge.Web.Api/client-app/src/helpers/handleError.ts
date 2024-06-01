export function handleError(e: any) {
    if (e.status === 404) {
        window.location.replace(`/notFound`);
    }
}