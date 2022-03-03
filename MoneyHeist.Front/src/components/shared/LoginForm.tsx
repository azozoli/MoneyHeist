import React from 'react';
import { getTranslate, TranslateFunction } from 'react-localize-redux';
import { useDispatch, useSelector } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { Form, FormElement, Field } from '@progress/kendo-react-form';
import { Input } from '@progress/kendo-react-inputs';
import { Error } from '@progress/kendo-react-labels';
import { Dialog } from '@progress/kendo-react-dialogs';
import { login } from '../../stores/shared/actions';
import { history } from '../../helper/configure/history';

interface IStateProps {
    translate: TranslateFunction;
}

const LoginFormComponent: React.FC = () => {
    const dispatch = useDispatch();
    const { translate } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
        };
    });

    const emailRegex = new RegExp(/\S+@\S+\.\S+/);

    const emailValidator = (value: any) => (emailRegex.test(value) ? '' : 'Please enter a valid email.');

    const EmailInput = (fieldRenderProps: any) => {
        const { validationMessage, visited, ...others } = fieldRenderProps;
        return (
            <div>
                <Input {...others} />
                {visited && validationMessage && <Error>{validationMessage}</Error>}
            </div>
        );
    };

    const handleSubmit = (dataItem: any) => {
        dispatch(login({ email: dataItem.email, password: dataItem.password }));
        history.push('/test');
    };

    return (
        <>
            <Dialog width={'30em'} height={'20em'} style={{ margin: '0px' }} title={translate('login').toString()}>
                <div style={{ margin: 'auto', width: '50%' }}>
                    <Form
                        onSubmit={handleSubmit}
                        render={(formRenderProps) => (
                            <FormElement style={{ maxWidth: 650 }}>
                                <fieldset className={'k-form-fieldset'}>
                                    <legend className={'k-form-legend'}>Please fill in the fields:</legend>
                                    <div className="mb-3">
                                        <Field name={'email'} type={'email'} component={EmailInput} label={'Email'} validator={emailValidator} />
                                    </div>
                                    <div className="mb-3">
                                        <Field name={'password'} component={Input} label={'Password'} />
                                    </div>
                                    <div style={{ margin: 'auto', width: '50%' }} className="k-form-buttons">
                                        <button type={'submit'} className="k-button" disabled={!formRenderProps.allowSubmit}>
                                            {translate('login')}
                                        </button>
                                    </div>
                                </fieldset>
                            </FormElement>
                        )}
                    />
                </div>
            </Dialog>
        </>
    );
};
export default LoginFormComponent;
