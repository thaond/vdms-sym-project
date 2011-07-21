/// <reference name="MicrosoftAjax.js"/>

/* Favourite Part */
function Starring(PartNo, Marked, star) { FavouritePartService.MarkPart(PartNo, Marked, OnSucceededStarring, OnFailedStarring, star, null); }
function OnSucceededStarring(result, star, methodName) {
	if (star.className == "gmstar") star.className = "gmnostar";
	else star.className = "gmstar";
}
function OnFailedStarring() { }
