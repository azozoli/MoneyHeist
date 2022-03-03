import React, { Dispatch, SetStateAction } from 'react';
import { Dialog } from '@progress/kendo-react-dialogs';
import { Form, Field, FormElement, FieldRenderProps } from '@progress/kendo-react-form';
import { getTranslate, TranslateFunction } from 'react-localize-redux';
import { useSelector } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { Button } from '@progress/kendo-react-buttons';
import { FormInputField } from './FunctionComponents/Inputs';
import { isFieldEmptyValidator } from '../../helper/dataValidations';
import { CustomDropDownList, EnumDropDownList } from './FunctionComponents/DropdownLists';
import { DatePickerComponent } from './FunctionComponents/DatePickers';

interface IStateProps {
    translate: TranslateFunction;
}

interface IProps {
    dataItem: any;
    formItems: any[];
    updateHandler: (dataItem: any, updateAction: Dispatch<any>) => void;
    updateReduxAction: Dispatch<any>;
    createHandler: (dataItem: any, createAction: Dispatch<any>) => void;
    createReduxAction: Dispatch<any>;
    toggleGridFormHandler: Dispatch<SetStateAction<{ isOpen: boolean; type: string; dataItem: any }>>;
    formType: string;
}

const EditForm: React.FC<IProps> = ({
    dataItem,
    formItems,
    updateHandler,
    updateReduxAction,
    toggleGridFormHandler,
    createHandler,
    createReduxAction,
    formType,
}: IProps) => {
    const { translate } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
        };
    });

    const FieldComponentRender = (fieldRenderProps: FieldRenderProps) => {
        switch (fieldRenderProps.type) {
            case 'input':
                return FormInputField(fieldRenderProps);
            case 'dropdown':
                return CustomDropDownList(fieldRenderProps);
            case 'enumDropdown':
                return EnumDropDownList(fieldRenderProps);
            case 'date':
                return DatePickerComponent(fieldRenderProps, undefined, undefined, 'datePicker');
            default:
                return FormInputField(fieldRenderProps);
        }
    };

    if (typeof dataItem.value === 'string' && dataItem.propEnums && dataItem.propEnums.length > 0) {
        dataItem.value = dataItem.propEnums.filter((e: any) => e.value === dataItem.value)[0];
    }

    return (
        <Dialog
            title={dataItem && dataItem.inEdit ? `${translate('edit')} ${dataItem.name}` : `${translate('addNew')}`}
            onClose={() => toggleGridFormHandler({ isOpen: false, type: '', dataItem: {} })}
        >
            <Form
                initialValues={dataItem}
                onSubmit={(dataItem) =>
                    formType === 'create' ? createHandler(dataItem, createReduxAction) : updateHandler(dataItem, updateReduxAction)
                }
                render={(formRenderProps) => (
                    <FormElement style={{ maxWidth: 650, minWidth: 200 }}>
                        <fieldset className={'k-form-fieldset'}>
                            {formItems &&
                                formItems.map((fitem, index) => {
                                    if (fitem.type === 'propertyValue') {
                                        return (
                                            <div className="mb-3 gridEditFormField" key={index}>
                                                <label style={{ display: 'block' }}>
                                                    <b>{fitem.label}</b>
                                                </label>
                                                <Field
                                                    name={fitem.name}
                                                    textField={'displayName'}
                                                    dataItemKey={'value'}
                                                    disabled={fitem.disabled}
                                                    component={FieldComponentRender}
                                                    data={dataItem.propEnums}
                                                    type={fitem.type}
                                                    validator={fitem.required && dataItem.typeName !== 'BOOLEAN' ? isFieldEmptyValidator : undefined}
                                                />
                                            </div>
                                        );
                                    } else if (fitem.onChange)
                                        return (
                                            <div className="mb-3 gridEditFormField" key={index}>
                                                <label style={{ display: 'block' }}>
                                                    <b>{fitem.label}</b>
                                                </label>
                                                <Field
                                                    name={fitem.name}
                                                    onChange={(e: any) => {
                                                        fitem.onChange(e);
                                                    }}
                                                    textField={fitem.textField}
                                                    dataItemKey={fitem.dataItemKey}
                                                    disabled={fitem.disabled}
                                                    component={FieldComponentRender}
                                                    data={fitem.data}
                                                    validator={fitem.required ? isFieldEmptyValidator : undefined}
                                                />
                                            </div>
                                        );
                                    else
                                        return (
                                            <div className="mb-3 gridEditFormField" key={index}>
                                                <label style={{ display: 'block' }}>
                                                    <b>{fitem.label}</b>
                                                </label>
                                                <Field
                                                    name={fitem.name}
                                                    type={fitem.type}
                                                    component={FieldComponentRender}
                                                    disabled={fitem.disabled}
                                                    validator={fitem.required ? isFieldEmptyValidator : undefined}
                                                />
                                            </div>
                                        );
                                })}
                        </fieldset>
                        <div className="k-form-buttons">
                            <button type={'submit'} className="k-button k-primary" disabled={!formRenderProps.allowSubmit}>
                                {formType === 'create' ? translate('add') : translate('update')}
                            </button>
                            <Button
                                onClick={() => toggleGridFormHandler({ isOpen: false, type: '', dataItem: {} })}
                                className="k-button"
                                style={{ marginLeft: '10px' }}
                            >
                                {translate('cancel')}
                            </Button>
                        </div>
                    </FormElement>
                )}
            />
        </Dialog>
    );
};
export default EditForm;
