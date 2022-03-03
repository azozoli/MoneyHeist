import React from 'react';
import { Input } from '@progress/kendo-react-inputs';
import { FieldRenderProps } from '@progress/kendo-react-form';
//import { Error } from '@progress/kendo-react-labels';
export const FormInputField = (fieldRenderProps: FieldRenderProps) => {
    const { validationMessage, visited, ...others } = fieldRenderProps;
    return (
        <>
            <Input {...others} />
            {/*  {visited && validationMessage && <Error>{validationMessage}</Error>} */}
        </>
    );
};
