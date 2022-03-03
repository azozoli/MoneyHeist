function getPublicSettings() {
	return ${APP_PUBLIC_SETTINGS};
}

window.onload = function() {
	setAppTitle();
	setAppVersion();
	
	function setAppTitle() {
		document.title = "${ADNET_APP_TITLE}";
	}

	function setAppVersion() {
		document.getElementById("adnet-version").innerHTML = "Adnet ${ADNET_APP_TITLE} v.${ADNET_APP_VERSION}";
	}
}