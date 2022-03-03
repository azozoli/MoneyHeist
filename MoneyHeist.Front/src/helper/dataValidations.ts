//Form field validation
export const isFieldEmptyValidator = (value: string): string => {
    return value ? '' : 'Field must be filled';
};
