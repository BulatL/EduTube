$(function () {
	$("#imageUpload").change(function () {
		fasterPreview(this);
	});
	let oldImage = $('#oldImage').val();
	$('#imagePreview').attr('src', `/images/${oldImage}`);
});
function fasterPreview(uploader) {
	if (uploader.files && uploader.files[0]) {
		$('#imagePreview').attr('src',
			window.URL.createObjectURL(uploader.files[0]));
	}
}

function DeleteEmoji(id) {
	console.log("usao");
	$("#deleteDialog").load(`/Emojis/GetDeleteDialog/${id}`, function (responseTxt, statusTxt, xhr) {
		if (statusTxt === "error")
			console.log("error");
		else {
			$('#deleteDialog').modal('show');
		}
	});
}

function DeleteEmojiConfirm(id) {
	$.ajax({
		url: `/Emojis/Delete/${id}`,
		type: 'DELETE',
		dataType: 'json',
		success: function (response) {
			console.log(response);
		},
		error: function (data, xhr) {
			if (data.status === 200)
				$(`#emoji_${id}`).remove();

			else
				alert('Delete operation failed');


			$('#deleteDialog').modal('hide');
		}
	});
}