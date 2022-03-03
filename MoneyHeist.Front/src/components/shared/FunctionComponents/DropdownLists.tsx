import React from 'react';
import { FieldWrapper, FieldRenderProps } from '@progress/kendo-react-form';
import { DropDownList } from '@progress/kendo-react-dropdowns';
//import { enumToValue } from '../../../models/Enum';

export const EnumDropDownList = (fieldRenderProps: FieldRenderProps) => {
    const {
        validationMessage,
        id,
        disabled,
        defaultValue,
        dataItemKey,
        textField,
        wrapperStyle,
        data,
        onChange,
        changeHandler,
        value,
        enumForChangeHandler,
        ...others
    } = fieldRenderProps;

    return (
        <FieldWrapper style={wrapperStyle}>
            <DropDownList
                disabled={disabled}
                value={value}
                defaultValue={defaultValue}
                data={data}
                dataItemKey={dataItemKey}
                textField={textField}
                onChange={(e) => {
                    //changeHandler(enumToValue(enumForChangeHandler, e.target.value[textField]));
                    onChange(e);
                }}
                {...others}
            />
        </FieldWrapper>
    );
};

export const CustomDropDownList = (fieldRenderProps: FieldRenderProps) => {
    const { validationMessage, id, disabled, defaultValue, dataItemKey, wrapperStyle, data, onChange, changeHandler, value, ...others } =
        fieldRenderProps;

    return (
        <FieldWrapper style={wrapperStyle}>
            <DropDownList
                //id={id}
                disabled={disabled}
                value={typeof value === 'object' && value.length > 0 ? value[0] : value}
                defaultValue={defaultValue}
                data={data}
                onChange={(e) => {
                    changeHandler && changeHandler(dataItemKey === 'id' ? e.value.id : e.value);
                    onChange(e);
                }}
                {...others}
            />
        </FieldWrapper>
    );
};

export const CustomFormDropDownList = (fieldRenderProps: FieldRenderProps) => {
    const { validationMessage, id, disabled, defaultValue, dataItemKey, wrapperStyle, data, onChange, changeHandler, value, ...others } =
        fieldRenderProps;

    return (
        <FieldWrapper style={wrapperStyle}>
            <DropDownList
                //id={id}
                disabled={disabled}
                defaultValue={defaultValue}
                data={data}
                onChange={(e) => {
                    changeHandler && changeHandler(dataItemKey === 'id' ? e.value.id : e.value);
                    onChange(e);
                }}
                {...others}
            />
        </FieldWrapper>
    );
};
