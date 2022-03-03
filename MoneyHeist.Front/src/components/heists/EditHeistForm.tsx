import React, { useState } from 'react';
import { Form, Field, FormElement } from '@progress/kendo-react-form';
import { Input } from '@progress/kendo-react-inputs';
import { Window } from '@progress/kendo-react-dialogs';
import { IHeist } from '../../models/Heist';
import { useDispatch, useSelector } from 'react-redux';
import { showEditHeistFormWindow, updateHeist } from '../../stores/heist/actions';
import { getTranslate, TranslateFunction } from 'react-localize-redux';
import { AppState } from '../../stores/rootReducer';

interface IProps {
    heistDetails: IHeist;
}

interface IStateProps {
    translate: TranslateFunction;
}

const EditHeistForm: React.FC<IProps> = ({ heistDetails }: IProps) => {
    const dispatch = useDispatch();

    const { translate } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
            heistEditFormWindowIsOpen: state.heists.heistEditFormWindowIsOpen,
        };
    });

    const [windowWidth, setWindowWidth] = useState(420);
    const [, setScreenWidth] = useState(window.innerWidth);
    window.addEventListener('resize', () => setScreenWidth(window.innerWidth));

    return (
        <div className="custom-window">
            <Window
                width={windowWidth}
                initialHeight={window.innerHeight * 0.9}
                initialTop={20}
                onResize={(e: any) => setWindowWidth(e.width)}
                onClose={() => dispatch(showEditHeistFormWindow(false))}
                title={translate('heist') + ' ' + heistDetails.name}
            >
                <div className="window-content">
                    <Form
                        initialValues={heistDetails}
                        onSubmit={(dataItem) => {
                            dispatch(updateHeist(dataItem));
                        }}
                        render={(formRenderProps) => (
                            <FormElement>
                                <fieldset className={'k-form-fieldset'}>
                                    <label className="form-label">{translate('name')}:</label>
                                    <div className="col-6 form-element">
                                        <Field name={'name'} component={Input} />
                                    </div>
                                    <label className="form-label">{translate('location')}:</label>
                                    <div className="col-6 form-element">
                                        <Field name={'location'} component={Input} />
                                    </div>
                                    <label className="form-label">{translate('startTime')}:</label>
                                    <div className="col-6 form-element">
                                        <Field name={'startTime'} component={Input} />
                                    </div>
                                    <label className="form-label">{translate('endTime')}:</label>
                                    <div className="col-6 form-element">
                                        <Field name={'endTime'} component={Input} />
                                    </div>
                                </fieldset>
                                <div className="k-form-buttons">
                                    <button type={'submit'} className="k-button" disabled={!formRenderProps.allowSubmit}>
                                        {translate('submit')}
                                    </button>
                                </div>
                            </FormElement>
                        )}
                    />
                </div>
            </Window>
        </div>
    );
};
export default EditHeistForm;
