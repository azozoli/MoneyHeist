import React from 'react';
import { FieldRenderProps } from '@progress/kendo-react-form';

export const DatePickerComponent = (
    fieldRenderProps: FieldRenderProps,
    minValue: any,
    maxValue: any,
    datePickerType: 'datePicker' | 'datePickerGrid',
) => {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const { validationMessage, visited, ...others } = fieldRenderProps;
    return (
        <>
            {/*  <CustomDatePicker
                fieldRenderProps={fieldRenderProps}
                minValue={minValue}
                maxValue={maxValue}
                datePickerType={datePickerType}
                defaultValue={fieldRenderProps.value}
            /> */}
            {/*  {visited && validationMessage && <Error>{validationMessage}</Error>} */}
        </>
    );
};
