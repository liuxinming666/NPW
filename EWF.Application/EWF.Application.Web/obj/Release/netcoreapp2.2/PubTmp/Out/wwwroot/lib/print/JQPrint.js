﻿(function ($) {
    var printAreaCount = 0;
        var ele = $(this);
            style: iframeStyle,
        });
        //    return $(this).attr("rel").toLowerCase() == "stylesheet";
        //}).each(
        //            doc.write('<link type="text/css" rel="stylesheet" href="'
        //        });
    }
        $("iframe#" + id).remove();
    };
        var tableString = '<table cellspacing="0" class="pb">';
            $(columns).each(function (index) {
                tableString += '\n<tr>';
                    for (var i = 0; i < frozenColumns[index].length; ++i) {
                        if (!frozenColumns[index][i].hidden) {
                            tableString += '\n<th width="' + frozenColumns[index][i].width + '"';
                                tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                            }
                                tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                            }
                                nameList += ',{"f":"' + frozenColumns[index][i].field + '", "a":"' + frozenColumns[index][i].align + '"}';
                            }
                        }
                    }
                }
                    if (!columns[index][i].hidden) {
                        tableString += '\n<th width="' + columns[index][i].width + '"';
                            tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                        }
                            tableString += ' colspan="' + columns[index][i].colspan + '"';
                        }
                            nameList += ',{"f":"' + columns[index][i].field + '", "a":"' + columns[index][i].align + '"}';
                        }
                    }
                }
            });
        }
            tableString += '\n<tr>';
                var e = nl[j].f.lastIndexOf('_0');
                    tableString += ' style="text-align:' + nl[j].a + ';"';
                }
                //    tableString += (rows[i][nl[j].f.substring(0, e)]==null?"":rows[i][nl[j].f.substring(0, e)]);
                //}
            });
        }
    }
})(jQuery);