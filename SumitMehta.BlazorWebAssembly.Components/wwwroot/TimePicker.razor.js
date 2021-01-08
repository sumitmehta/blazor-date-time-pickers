/**
 * Get the bounding client rectangle for the passed element
 * @param {any} element The element for which we need to calculate the bounds
 * @returns The bounding client rectangle for the passed element
 */
export function GetBoundingClientRect(element) {
    return element.getBoundingClientRect();
}