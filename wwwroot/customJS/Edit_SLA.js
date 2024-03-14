


    $(function () {

        $('#btnFileUpload').click(function (event) {


            if (window.FormData !== undefined) {

                var fileUpload = $("#FileUpload_Id").get(0);
                var files = fileUpload.files;

                var fileRefId = $("#SLA_Id").val();
                //var fileId = $("#File_Name").val();
                var fileId = $('#abcdefgh').html();
               

                // Create FormData object
                var fileData = new FormData();

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                fileData.append('SLA_Id', fileRefId);
                fileData.append('file_Id', fileId);

                $.ajax({
                    type: "POST",
                    url: "/SLA/FiliUpload",
                    datatype: "Json",
                    data: fileData,
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    success: function (response) {

                        alert('SLA File has been replaced successfully');
                        //$("#Files_Grid tr").remove();

                        //markup = "<tr><th style='width: 50%;'> File Name </th> <th style='width: 25%;'> Download </th> <th style='width: 25%; text-align: center'> Delete </th> </tr>";
                        //tableBody = $("#Files_Grid tbody");
                        //tableBody.append(markup);


                        if (response != null) {

                            $.each(response, function (key, val) {

                                //markup = "<tr><td> " + val.File_Name + " </td> " +
                                //    "<td>  <input type='submit' value='Download' class='cssdownload btn_download' id='" + val.File_Id + "' > </input> </td> " +
                                //    "<td style=' text-align: center'>  <input type='submit' class='appDetails btn_delete' id='" + val.File_Id + "' value='Delete' > </input> </td></tr> "

                                //tableBody = $("#Files_Grid tbody");
                                //tableBody.append(markup);

                                

                            });
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }

                });
            }

            event.preventDefault();

        });
    });
    $(function () {

        $(document).on("click", ".appDetails", function () {
            var clickedBtnID = $(this).attr('id');

            var fileRefId = $("#SLA_Id").val();

            if (confirm('Are you sure you want to delete this?')) {

                $.ajax({
                    type: "POST",
                    url: "/SLA/DeleteFile",
                    datatype: "Json",
                    data: { FileId: clickedBtnID, RefId: fileRefId },
                    success: function (response) {


                        $("#Files_Grid tr").remove();

                        markup = "<tr><th style='width: 50%;'> File Name </th> <th style='width: 25%;'> Download </th> <th style='width: 25%; text-align: center'> Delete </th> </tr>";
                        tableBody = $("#Files_Grid tbody");
                        tableBody.append(markup);


                        if (response != null) {

                            $.each(response, function (key, val) {

                                markup = "<tr><td> " + val.File_Name + " </td> " +
                                    "<td>  <input type='submit' value='Download' class='cssdownload btn_download' id='" + val.File_Id + "' > </input> </td> " +
                                    "<td style=' text-align: center'>  <input type='submit' class='appDetails btn_delete' id='" + val.File_Id + "' value='Delete' > </input> </td></tr> "

                                tableBody = $("#Files_Grid tbody");
                                tableBody.append(markup);

                            });
                        }


                    },
                    error: function (err) {
                        alert(err.statusText);
                    }

                });

            }

            event.preventDefault();

        });
    });

    $(document).on("click", ".cssdownload", function () {
        var fileName = $(this).attr('id');


        window.location.href = "/SLA/Download?fileId=" + fileName + "";


        event.preventDefault();
    });


    
