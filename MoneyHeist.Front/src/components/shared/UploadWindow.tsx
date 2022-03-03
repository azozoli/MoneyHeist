import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { Window } from '@progress/kendo-react-dialogs';
import { Upload, UploadOnBeforeUploadEvent, UploadOnStatusChangeEvent } from '@progress/kendo-react-upload';
import frontendSettings from '../../helper/frontendSettings';
import { showUploadWindow } from '../../stores/shared/actions';
import { GOdsMsgType } from '../../models/Enums';

interface IUploadWindowComponentProps {
	tabType: GOdsMsgType;
	billingGroupId: number;
}

const UploadWindow: React.FC<IUploadWindowComponentProps> = (tabType, billingGroupId) => {
	const dispatch = useDispatch();

	const [windowWidth, setWindowWidth] = useState(820);
	const [screenWidth] = useState(window.innerWidth);

	const [readingType] = useState(tabType.tabType);

	const onBeforeUpload = (event: UploadOnBeforeUploadEvent) => {
		//event.headers.Authorization = 'Bearer ' + verifiedUser.token;
	};

	return (
		<div className="custom-window">
			<Window
				width={windowWidth * 1}
				initialHeight={window.innerHeight * 0.52}
				initialLeft={screenWidth * 0.27}
				initialTop={60}
				onClose={() => {
					dispatch(showUploadWindow(false));
				}}
				onResize={(e: any) => setWindowWidth(e.width)}
			>
				<div className="html-editor-div">
					<Upload
						defaultFiles={[]}
						withCredentials={false}
						saveField="fileContent"
						autoUpload={false}
						restrictions={{
							allowedExtensions: ['.xlsx'],
						}}
						onStatusChange={(event: UploadOnStatusChangeEvent) => {
							//console.log(event);
						}}
						onBeforeUpload={onBeforeUpload}
						saveUrl={`${frontendSettings.ApiUrl}/Import/ImportReadingForBilling?type=${readingType}&billingGroupId=${tabType.billingGroupId}`}
					/>
				</div>
			</Window>
		</div>
	);
};

export default UploadWindow;
